package de.jgoedde.pubgreportcrawler;

import de.jgoedde.pubgreportcrawler.config.AppConfig;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.ApplicationArguments;
import org.springframework.boot.ApplicationRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.context.properties.EnableConfigurationProperties;

@EnableConfigurationProperties(AppConfig.class)
@SpringBootApplication
public class PubgReportCrawlerApplication implements ApplicationRunner {
    public static void main(String[] args) {
        SpringApplication.run(PubgReportCrawlerApplication.class, args);
    }

    @Override
    public void run(ApplicationArguments args) throws Exception {
        if (!args.containsOption("account")) {
            throw new IllegalArgumentException("You must provide at least one account to watch for");
        }

        if (!args.containsOption("discord-id")) {
            throw new IllegalArgumentException("You must provide a discord user id");
        }
    }
}
