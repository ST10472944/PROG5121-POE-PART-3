# PROG5121-POE-PART-3
# Cybersecurity Awareness Bot – POE (Part 3)

## Overview
This is a WPF-based cybersecurity awareness chatbot that includes:
- **Cybersecurity tips** (keyword recognition, random tips, sentiment detection, memory)
- **Task Assistant** (add, delete, mark complete, optional reminder date – stored in MySQL)
- **Cybersecurity Quiz** (12 questions, multiple-choice/true-false, feedback, score)
- **Activity Log** (tracks all actions, shows last 10 with "Show More")
- **NLP Simulation** (chat commands: `add task`, `start quiz`, `show log`, `help`)

## How to Run
1. Ensure MySQL Server is running and the database `cyberbot` exists (see SQL script in the documentation).
2. Open the solution in Visual Studio.
3. Update the connection string in `DatabaseHelper.cs` with your MySQL root password.
4. Build and run (F5).

## Video Presentation


## CI Workflow


## GitHub Tags / Releases
- v1.0 – Console chatbot (Part 1) 
- v2.0 – WPF chatbot (Part 2)
- v3.0 – POE full version (Part 3)

## Author
 Makgai Thapelo Mmoloki ST1042944
