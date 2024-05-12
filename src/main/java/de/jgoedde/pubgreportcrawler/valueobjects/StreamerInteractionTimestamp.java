package de.jgoedde.pubgreportcrawler.valueobjects;

import java.time.Instant;

/**
 * A value object representing a point in time when a streamer has interacted with a player.
 * @param value The point in time when the interaction occurred.
 */
public record StreamerInteractionTimestamp(Instant value) {
}
