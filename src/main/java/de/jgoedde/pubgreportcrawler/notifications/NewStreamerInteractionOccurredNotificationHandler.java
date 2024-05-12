package de.jgoedde.pubgreportcrawler.notifications;

import an.awesome.pipelinr.Notification;
import lombok.extern.java.Log;
import org.springframework.stereotype.Component;

@Component
@Log
public class NewStreamerInteractionOccurredNotificationHandler implements Notification.Handler<NewStreamerInteractionOccurredNotification> {
    @Override
    public void handle(NewStreamerInteractionOccurredNotification notification) {
        log.info("Should send discord message to Me");
        // TODO: Send discord DM
    }
}
