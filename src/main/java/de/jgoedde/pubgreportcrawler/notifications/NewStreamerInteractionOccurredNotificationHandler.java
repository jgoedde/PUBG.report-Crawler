package de.jgoedde.pubgreportcrawler.notifications;

import an.awesome.pipelinr.Notification;
import de.jgoedde.pubgreportcrawler.config.DiscordBotConfig;
import discord4j.common.util.Snowflake;
import discord4j.core.object.entity.User;
import discord4j.core.spec.MessageCreateSpec;
import lombok.RequiredArgsConstructor;
import lombok.extern.java.Log;
import org.springframework.boot.ApplicationArguments;
import org.springframework.stereotype.Component;

@Component
@Log
@RequiredArgsConstructor
public class NewStreamerInteractionOccurredNotificationHandler implements Notification.Handler<NewStreamerInteractionOccurredNotification> {
    private final DiscordBotConfig discordBot;
    private final ApplicationArguments args;

    @Override
    public void handle(NewStreamerInteractionOccurredNotification notification) {
        String message = "neue Streamer-Interaktionen: https://pubg.report/players/" + notification.kraftonAccountId();
        long discordUserId = Long.parseLong(args.getOptionValues("discord-id").getFirst());

        discordBot.gatewayDiscordClient().getUserById(Snowflake.of(discordUserId))
                .flatMap(User::getPrivateChannel)
                .flatMap(channel -> channel.createMessage(MessageCreateSpec.builder()
                        .content(message)
                        .build()))
                .subscribe();
    }
}