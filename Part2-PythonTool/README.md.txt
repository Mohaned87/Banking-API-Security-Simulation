# Part 2 – Custom Python Security Tool

This part introduces a Python-based tool to perform automated security testing 
on the banking API.

## Features
- Brute-force login attempts using a wordlist.
- SQL injection testing with `' OR '1'='1`.
- Logs successful attempts (email, password, JWT token).
- Saves results to `results.txt`.

## Files
- `tool.py` – the Python script.
- `pass.txt` – sample wordlist.
- `results.txt` – example output.
- Report.pdf – full documentation.
- Screenshots/ – supporting images.

## Example Usage
```bash
python3 tool.py
