package de.jgoedde.pubgreportcrawler.valueobjects;

/**
 * A value object representing Krafton's PUBG account identifier.
 * @param value The ID of the account.
 */
public record KraftonAccountId(String value) {
    @Override
    public String toString() {
        return value;
    }
}
