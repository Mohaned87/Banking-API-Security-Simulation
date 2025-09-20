# Banking API Security Simulation

This repository documents a hands-on security project combining 
**.NET 9 Web API development** and **penetration testing techniques**.  

It includes two parts:

## 🔹 Part 1 – Hydra Attack on .NET 9 API
- Built a **custom banking API** with SQLite as the database.
- Simulated **brute-force attacks** using Hydra.
- Documented results with screenshots and security recommendations.

📄 [Full Report (PDF)](./https://github.com/Mohaned87/Banking-API-Security-Simulation/blob/main/Part1-Hydra-DotNetAPI/Part1-Hydra-DotNetAPI.pdf)

---

## 🔹 Part 2 – Custom Python Security Tool
- Developed a **Python 3 tool** to perform brute-force and SQL injection testing.
- Automated login attempts using a wordlist.
- Captured tokens, attempts, and execution time.
- Verified API protection against basic SQL injection payloads.

📄 [Full Report (PDF)](./https://github.com/Mohaned87/Banking-API-Security-Simulation/blob/main/Part2-PythonTool/Part2-PythonTool.pdf)

---

## 🚀 Next Steps
- Extend the Python tool with stronger wordlists.
- Compare weak vs. strong passwords in practice.
- Implement defensive measures (rate limiting, MFA, WAF).
- Share Phase 3: **Hardening the API**.

---

## 🛠️ Technologies
- **.NET 9 Web API (C#)**
- **SQLite**
- **Hydra**
- **Python 3**
