# Microsoft Store - Submission Options

## Notes for certification

This is a desktop productivity application built with WinUI 3 and .NET 8. The app works completely offline and stores all data locally on the user's device.

Testing instructions:
1. Install and launch the app
2. Click the "+" button to create a new task
3. Click on a task to start/pause the timer
4. Right-click on a task name to rename it
5. Drag tasks to reorder them
6. Close and reopen the app to verify data persistence

No special test accounts or configurations are required. The app is fully functional immediately after installation.

---

## Restricted Capabilities - runFullTrust

**Why do you need the runFullTrust capability, and how will it be used in your product?**

Required for WinUI 3 desktop apps (Windows App SDK standard).

Usage:
1. File System: Store task data locally in app data folder
2. Window Management: Create always-on-top desktop window
3. Logging: Write debug logs locally

The app does NOT access internet, user files, collect telemetry, need admin rights, or interact with other apps. Standard offline productivity tool.

---

## Telemetry & Data Collection

**Does your product collect, store, or transmit personal user information or telemetry?**

No

**Explanation:**

TaskTimerWidget is a 100% offline application that does not collect, store, or transmit any personal information or telemetry.

All user data (task names and timer data) is stored locally on the user's device in:
%LocalAppData%\TaskTimerWidget\Data\tasks.json

The app does not:
- Connect to the internet
- Send any data to external servers
- Collect analytics or usage statistics
- Track user behavior
- Require user accounts or authentication
- Access any personal information

The privacy policy is available at:
https://melihcelenk.github.io/TaskTimerWidget/PRIVACY_POLICY.html
