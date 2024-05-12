package de.jgoedde.pubgreportcrawler;

import de.jgoedde.pubgreportcrawler.config.AppConfig;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.context.properties.EnableConfigurationProperties;

@EnableConfigurationProperties(AppConfig.class)
@SpringBootApplication
public class PubgReportCrawlerApplication {

	public static void main(String[] args) {
		SpringApplication.run(PubgReportCrawlerApplication.class, args);
	}

}
