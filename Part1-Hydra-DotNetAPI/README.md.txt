# Part 1 – Hydra Attack on .NET 9 API

This part documents the creation of a custom **.NET 9 Web API** with SQLite 
and the simulation of **brute-force login attacks** using Hydra.

## Contents
- Report.pdf – full documentation with screenshots.
- Screenshots/ – images referenced in the report.

## Key Findings
- Weak passwords (e.g., `123456`) are easily brute-forced.
- Lack of rate limiting allows repeated login attempts.
- Security hardening is required.

## Recommendations
- Strong password policy.
- Account lockouts.
- Rate limiting and MFA.
