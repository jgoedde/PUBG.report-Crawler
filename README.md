# PUBG.report-Crawler

[![C#](https://custom-icon-badges.demolab.com/badge/C%23-%23239120.svg?logo=cshrp&logoColor=white)](#)
[![.NET](https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff)](#)

A script notifying users on new entries on https://pubg.report via discord private message.

Fetches the pubg.report API for new streams of one specific player ID and stores the latest match time within the lifetime of the application, without a database. Every 30 Minutes new matches are being checked for.

![image](https://github.com/jgoedde/PUBG.report-Crawler/assets/129423545/e28b12e9-ed9c-4e0c-8837-1a5b8596d835)

## How to run

This app is containerized using Docker.

ℹ️ Make sure git is installed.

### Using docker:

```
git clone https://github.com/jgoedde/PUBG.report-Crawler.git \
cd PUBG.report-Crawler
```

Copy the `.env.example` file and adjust the variables.

- `DISCORD_ACCOUNT_ID` - an 18 digits long number. It is used as the account that gets notified on new streams via direct message.
- `KRAFTON_ACCOUNT_ID` - the ID of the player whose streamer interactions should be tracked. In other words - *your* krafton account ID.
- `DISCORD_BOT_TOKEN` - the token of the discord bot that sends you the messages. You have to share a server with the bot.
- `PUBG_NICK` - your PUBG nickname. It is used to determine if you *have killed* a streamer or whether you *were killed* by a streamer.

Save the file under the name `.env` in the root directory of the cloned project.

Run `docker compose up`
