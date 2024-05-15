package de.jgoedde.pubgreportcrawler.config;

import lombok.Getter;
import lombok.RequiredArgsConstructor;
import org.springframework.boot.context.properties.ConfigurationProperties;

@ConfigurationProperties(prefix = "app")
@Getter
@RequiredArgsConstructor
public final class AppConfig {
    private final String pubgReportApiUrl;
    private final String discordToken;
}
