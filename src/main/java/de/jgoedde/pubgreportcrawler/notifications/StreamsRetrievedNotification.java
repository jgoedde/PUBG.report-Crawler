package de.jgoedde.pubgreportcrawler.notifications;

import an.awesome.pipelinr.Notification;
import de.jgoedde.pubgreportcrawler.valueobjects.KraftonAccountId;
import de.jgoedde.pubgreportcrawler.valueobjects.StreamerInteractionTimestamp;

import java.util.List;

/**
 * A notification informing the system that the information about streamer encounters has been successfully retrieved from PUBG.report.
 *
 * @param kraftonAccountId The account ID from which the encounters were queried.
 * @param streamerEncounters The retrieved interaction times.
 */
public record StreamsRetrievedNotification(
        KraftonAccountId kraftonAccountId,
        List<StreamerInteractionTimestamp> streamerEncounters) implements Notification {
}
