# Task Timer Widget

A lightweight, minimal task timer widget for Windows 11. Manage your tasks and track time with a simple, distraction-free interface.

## 🎯 Features

- **Minimal Design**: Widget-sized window that doesn't take up much space
- **Task Management**: Create, delete, and manage multiple tasks
- **Built-in Timer**: Track elapsed time for each task
- **Active Task Highlighting**: Yellow highlight shows which task is currently active
- **Pause & Resume**: Click to pause/resume timer on any task
- **Persistent Storage**: Tasks are saved locally
- **Lightweight**: Minimal resource usage

## 🚀 Getting Started

### Prerequisites
- Windows 10/11 (Build 17763 or later)
- .NET 8.0 Runtime

### Installation

1. Clone or download the project
2. Open `src/TaskTimerWidget/TaskTimerWidget.csproj` in Visual Studio 2022
3. Build the project (Build > Build Solution)
4. Run the application (F5 or Debug > Start Debugging)

### Usage

1. **Add Task**: Click the `+` button to add a new task
2. **Name Task**: Enter the task name in the dialog and click Create
3. **Start Timer**: Click on a task to activate it and start the timer
4. **Pause Timer**: Click the active task again to pause the timer
5. **Switch Tasks**: Click another task to pause the current one and activate the new one
6. **Delete Task**: Click the `✕` button on a task to remove it

## 📁 Project Structure

```
TaskTimerWidget/
├── src/
│   └── TaskTimerWidget/
│       ├── Models/              # Data models (Task, etc.)
│       ├── ViewModels/          # MVVM ViewModels
│       ├── Services/            # Business logic services
│       ├── Helpers/             # Utility classes and converters
│       ├── Views/               # XAML UI files
│       ├── Assets/              # Images and resources
│       ├── App.xaml(.cs)        # Application entry point
│       └── TaskTimerWidget.csproj
├── CLAUDE.md                    # Code standards and guidelines
├── TODO.md                      # Development roadmap
├── MARKET_RESEARCH.md           # Market analysis and growth plan
└── README.md                    # This file
```

## 🏗️ Architecture

This project follows the **MVVM (Model-View-ViewModel)** pattern:

- **Models**: Pure data objects (Task.cs)
- **ViewModels**: Business logic and UI state management
- **Views**: XAML UI and code-behind
- **Services**: Data persistence and task management

## 🔧 Technology Stack

- **Framework**: .NET 8.0
- **UI**: WinUI 3
- **Logging**: Serilog
- **DI**: Microsoft.Extensions.DependencyInjection
- **Serialization**: System.Text.Json

## 📋 Development

### Building

```bash
dotnet build src/TaskTimerWidget/TaskTimerWidget.csproj
```

### Running

```bash
dotnet run --project src/TaskTimerWidget/TaskTimerWidget.csproj
```

### Code Standards

Please refer to [CLAUDE.md](./CLAUDE.md) for:
- Naming conventions
- Code style guidelines
- MVVM patterns
- Error handling standards
- Documentation requirements

### Development Roadmap

See [TODO.md](./TODO.md) for the complete development roadmap with phases and milestones.

## 📊 Market Analysis

See [MARKET_RESEARCH.md](./MARKET_RESEARCH.md) for:
- Market analysis and competition
- Growth strategies
- Monetization plan
- Timeline and projections

## 🐛 Known Issues

- Initial version (0.1.0)
- Some UI converters need additional testing
- Theme switching not yet implemented

## 🎯 Roadmap

### v0.2 (Next)
- [ ] Improved UI/UX
- [ ] Statistics dashboard
- [ ] Cloud sync preparation

### v1.0 (Release Candidate)
- [ ] Windows Store submission
- [ ] Performance optimization
- [ ] Multi-language support

### v2.0+ (Future)
- [ ] Cloud synchronization
- [ ] Team features
- [ ] Advanced analytics

## 📄 License

[Add your license here]

## 👥 Contributing

Contributions are welcome! Please:
1. Follow the code standards in CLAUDE.md
2. Create a feature branch
3. Submit a pull request with description

## 📞 Support

For issues, questions, or suggestions, please:
1. Check existing issues
2. Create a new issue with detailed description
3. Include screenshots/logs if applicable

## 🙏 Acknowledgments

- Built with WinUI 3 and .NET 8.0
- Inspired by Toggl and other time-tracking tools
- Designed for productivity enthusiasts

---

**Version**: 0.1.0
**Last Updated**: October 27, 2024
**Status**: Active Development
