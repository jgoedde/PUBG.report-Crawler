# PUBG.report-Crawler

Stateful Console App notifying users on new entries on https://pubg.report

Fetches the pubg.report API for new streams of one specific player ID and stores the latest match time within the lifetime of the application, without a database. Every 30 Minutes new matches are being checked for.

![image](https://github.com/jgoedde/PUBG.report-Crawler/assets/129423545/e28b12e9-ed9c-4e0c-8837-1a5b8596d835)

# How to run

This app is containerized using Docker.

ℹ️ Make sure Docker is installed.
ℹ️ Make sure git is installed.

## Setup by cloning with git:

```
git clone https://github.com/jgoedde/PUBG.report-Crawler.git \
cd PUBG.report-Crawler
```

Copy the `.env.example` file and adjust the variables.

- `DiscordAccountId` - an 18 digits long number. It is used as the account that gets notified on new streams via direct message.
- `KraftonAccountId` - the ID of the player whose streamer interactions should be tracked. In other words - *your* krafton account ID.
- `DiscordBotToken` - the token of the discord bot that sends you the messages. You have to share a server with the bot.

Save the file under the name `.env` in the root directory of the cloned project.

Run `docker compose up`
