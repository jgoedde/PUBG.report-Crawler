package de.jgoedde.pubgreportcrawler.config;

import discord4j.core.DiscordClientBuilder;
import discord4j.core.GatewayDiscordClient;
import jakarta.annotation.PreDestroy;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class DiscordBotConfig {
    private final String token;
    private GatewayDiscordClient client;

    public DiscordBotConfig(AppConfig properties) {
        this.token = properties.getDiscordToken();
    }

    @Bean
    public GatewayDiscordClient gatewayDiscordClient() {
        client = DiscordClientBuilder.create(token)
                .build()
                .login()
                .block();
        return client;
    }

    @PreDestroy
    public void logout() {
        if (client != null) {
            client.logout().block();
        }
    }
}
