# PUBG.report-Crawler

Stateful Console App notifying users on new entries on https://pubg.report

Fetches the pubg.report API for new streams of one specific player ID and stores the latest match time within the lifetime of the application, without a database. Every 30 Minutes new matches are being checked for.

![image](https://github.com/jgoedde/PUBG.report-Crawler/assets/129423545/e28b12e9-ed9c-4e0c-8837-1a5b8596d835)

# How to run

ℹ️ Make sure Docker is installed.

## Setup by cloning with git:

ℹ️ Make sure git is installed.

```
git clone https://github.com/jgoedde/PUBG.report-Crawler.git \
cd PUBG.report-Crawler \
docker build . -t pubg_report \
docker run -d pubg_report
```

## Setup manually:

Download [master.zip](https://github.com/jgoedde/PUBG.report-Crawler/archive/refs/heads/master.zip) and extract it's file contents.

```
cd PUBG.report-Crawler \
docker build . -t pubg_report \
docker run -d pubg_report
```
