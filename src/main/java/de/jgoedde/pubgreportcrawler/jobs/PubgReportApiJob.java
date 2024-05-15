package de.jgoedde.pubgreportcrawler.jobs;

import an.awesome.pipelinr.Pipeline;
import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import de.jgoedde.pubgreportcrawler.config.AppConfig;
import de.jgoedde.pubgreportcrawler.notifications.StreamsRetrievedNotification;
import de.jgoedde.pubgreportcrawler.valueobjects.KraftonAccountId;
import de.jgoedde.pubgreportcrawler.valueobjects.StreamerInteractionTimestamp;
import lombok.extern.slf4j.Slf4j;
import org.quartz.Job;
import org.quartz.JobExecutionContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.ApplicationArguments;
import org.springframework.stereotype.Component;

import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.time.Instant;
import java.util.HashMap;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ExecutionException;

@Slf4j
@Component
public class PubgReportApiJob implements Job {
    private final String pubgReportApiUrl;
    private final ObjectMapper objectMapper;
    private final Pipeline pipeline;
    private final ApplicationArguments args;

    @Autowired
    public PubgReportApiJob(AppConfig properties, ObjectMapper objectMapper, @Autowired Pipeline pipeline, ApplicationArguments args) {
        this.pubgReportApiUrl = properties.getPubgReportApiUrl();
        this.objectMapper = objectMapper;
        this.pipeline = pipeline;
        this.args = args;
    }

    @Override
    public void execute(JobExecutionContext jobExecutionContext) {
        HttpClient client = HttpClient.newHttpClient();

        KraftonAccountId kraftonAccountId = new KraftonAccountId(args.getOptionValues("account").getFirst());

        HttpRequest request = HttpRequest.newBuilder()
                .GET()
                .uri(URI.create(pubgReportApiUrl + "/players/" + kraftonAccountId + "/streams"))
                .build();

        try {
            String resStr = client.sendAsync(request, HttpResponse.BodyHandlers.ofString())
                    .thenApply(HttpResponse::body)
                    .get();

            GetStreamsResponse getStreamsResponse = objectMapper.readValue(resStr, GetStreamsResponse.class);

            List<StreamerInteractionTimestamp> streamerInteractionTimes = getStreamsResponse
                    .values().stream()
                    .flatMap(List::stream)
                    .map(it -> new StreamerInteractionTimestamp(it.timeEvent()))
                    .toList();

            StreamsRetrievedNotification notification = new StreamsRetrievedNotification(kraftonAccountId, streamerInteractionTimes);
            notification.send(pipeline);
        } catch (InterruptedException | ExecutionException e) {
            log.error("Something bad happened", e);
        } catch (JsonProcessingException e) {
            log.error("Encountered an error while deserializing the PUBG.report API response", e);
        } finally {
            client.close();
        }
    }

    public static class GetStreamsResponse extends HashMap<UUID, List<GetStreamsResponse.StreamResponse>> {
        @JsonIgnoreProperties(ignoreUnknown = true)
        public record StreamResponse(@JsonProperty("TimeEvent") Instant timeEvent) {
        }
    }
}
