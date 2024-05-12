package de.jgoedde.pubgreportcrawler.repositories;

import de.jgoedde.pubgreportcrawler.valueobjects.KraftonAccountId;
import de.jgoedde.pubgreportcrawler.valueobjects.StreamerInteractionTimestamp;

import java.util.Optional;

public interface StreamsRepository {
    /**
     * Retrieves the last interaction time of a streamer.
     *
     * @param kraftonAccountId The account ID of the player to check for his streamer interactions.
     * @return An Optional containing the last interaction time if it exists, otherwise an empty Optional.
     */
    Optional<StreamerInteractionTimestamp> getLastInteractionTime(KraftonAccountId kraftonAccountId);

    /**
     * Sets the last interaction time with a streamer.
     *
     * @param kraftonAccountId The account ID of the player that interacted with a streamer.
     * @param value The new most recent interaction time to be set.
     */
    void setLastInteractionTime(KraftonAccountId kraftonAccountId, StreamerInteractionTimestamp value);
}
