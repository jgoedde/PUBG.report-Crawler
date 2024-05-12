package de.jgoedde.pubgreportcrawler.repositories;

import de.jgoedde.pubgreportcrawler.valueobjects.KraftonAccountId;
import de.jgoedde.pubgreportcrawler.valueobjects.StreamerInteractionTimestamp;
import org.springframework.stereotype.Component;

import java.util.*;

/**
 * In-memory implementation of the StreamsRepository interface.
 * This class uses a HashMap to store the last interaction times of streamers.
 */
@Component
public class InMemStreamsRepository implements StreamsRepository {
    private final HashMap<KraftonAccountId, StreamerInteractionTimestamp> interactionTimes = new HashMap<>();

    @Override
    public Optional<StreamerInteractionTimestamp> getLastInteractionTime(KraftonAccountId kraftonAccountId) {
        if(!interactionTimes.containsKey(kraftonAccountId)) {
            return Optional.empty();
        }
        return Optional.of(interactionTimes.get(kraftonAccountId));
    }

    @Override
    public void setLastInteractionTime(KraftonAccountId kraftonAccountId, StreamerInteractionTimestamp value) {
        interactionTimes.put(kraftonAccountId, value);
    }
}