package de.jgoedde.pubgreportcrawler.notifications;

import an.awesome.pipelinr.Notification;
import an.awesome.pipelinr.Pipeline;
import de.jgoedde.pubgreportcrawler.valueobjects.StreamerInteractionTimestamp;
import de.jgoedde.pubgreportcrawler.repositories.StreamsRepository;
import lombok.RequiredArgsConstructor;
import lombok.extern.java.Log;
import org.springframework.stereotype.Component;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;
import java.util.Optional;

/**
 * Handles {@code StreamsRetrievedNotification} by checking if there are new streamer interactions since the last run.
 * If there are new encounters, the {@code  NewStreamerInteractionOccurredNotification} will be dispatched.
 */
@Log
@RequiredArgsConstructor
@Component
public class StreamsRetrievedNotificationHandler implements Notification.Handler<StreamsRetrievedNotification> {
    private final StreamsRepository streamsRepository;
    private final Pipeline pipeline;

    @Override
    public void handle(StreamsRetrievedNotification notification) {
        List<StreamerInteractionTimestamp> allEncounters = notification.streamerEncounters();
        Optional<StreamerInteractionTimestamp> lastEncounter = streamsRepository.getLastInteractionTime(notification.kraftonAccountId());

        // Find interaction times that are newer than the latest one saved in repository.
        List<StreamerInteractionTimestamp> moreRecentEncounters = new ArrayList<>(allEncounters.stream()
                .filter(it ->
                        lastEncounter.isEmpty() || it.value().isAfter(lastEncounter.get().value())
                )
                .toList());

        if (moreRecentEncounters.isEmpty()) {
            log.info("No newer streamer interactions found");
            return;
        }

        log.info("Found one or more newer streamer interactions");

        // Sort desc. so that the newest one appears first and we can save that newest entry.
        moreRecentEncounters.sort(Comparator.comparing(StreamerInteractionTimestamp::value).reversed());
        streamsRepository.setLastInteractionTime(notification.kraftonAccountId(), moreRecentEncounters.getFirst());

        new NewStreamerInteractionOccurredNotification(notification.kraftonAccountId()).send(pipeline);
    }
}
