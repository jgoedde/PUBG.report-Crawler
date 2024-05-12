package de.jgoedde.pubgreportcrawler.notifications;

import an.awesome.pipelinr.Notification;
import de.jgoedde.pubgreportcrawler.valueobjects.KraftonAccountId;

/**
 * A notification that informs the system that there have been new streamer encounters for a player since the last check.
 * @param kraftonAccountId The ID of the player that encountered a streamer in a match.
 */
public record NewStreamerInteractionOccurredNotification(KraftonAccountId kraftonAccountId) implements Notification {
}
