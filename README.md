# Weather Sensor Data Platform

This repository contains a complete pipeline for collecting and querying weather sensor data.  
It consists of two main components:

- **Python (ETL pipeline)** → Collects and stores weather data  
- **Server (C# API)** → Exposes the data for querying and consumption  

---

## Overview

The system is split into two responsibilities:

### 🐍 Python (Data Collection)

The Python project is responsible for:

- Extracting weather data from external APIs  
- Transforming the data into a consistent format  
- Loading the data into a PostgreSQL database  

In short: **this is the part that fills the database with sensor readings.**

### ⚙️ Server (C# API)

The Server project is responsible for:

- Providing an API to access the stored weather data  
- Enabling querying, filtering, and retrieval of sensor readings  

In short: **this is the part that lets you read and query the data.**

---

## Getting Started

To run the full system, you need to set up both parts.

### 1. Set up the Python ETL

Go to the Python folder and follow its [README](./python/README.md):

> **Note**  
> Small adjustments have been made to the Python code to ensure it works when running locally.  
> Because of this, the Docker setup described in the Python README may not work as expected.

### 2. Set up the Server (C# API)

Go to the Server folder and follow its [README](./Server/README.md):

---

## How It Fits Together

1. The **Python ETL** pulls weather data and stores it in the database.
2. The **Server API** reads from that database and exposes endpoints.
3. Clients can then query the API to access weather data.

---

## Summary

- Python = **writes data to the database**
- Server = **reads data from the database**
- Both parts must be configured for the system to work end-to-end
