package de.jgoedde.pubgreportcrawler.jobs;

import org.quartz.*;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class PubgReportApiJobConfig {
    @Bean
    public JobDetail httpRequestJobDetail() {
        return JobBuilder.newJob(PubgReportApiJob.class)
                .withIdentity("pubgReportJob")
                .storeDurably()
                .build();
    }

    @Bean
    public Trigger httpRequestJobTrigger() {
        SimpleScheduleBuilder scheduleBuilder = SimpleScheduleBuilder.simpleSchedule()
                .withIntervalInMinutes(30)
                .repeatForever();

        return TriggerBuilder.newTrigger()
                .forJob(httpRequestJobDetail())
                .withIdentity("pubgReportJobTrigger")
                .withSchedule(scheduleBuilder)
                .build();
    }
}