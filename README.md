🖥️ Virtual Machine Setup
The application was developed and tested inside a Windows 10 Virtual Machine.

If you’re using a VM (like VMware or VirtualBox), make sure the following are set up:

Network Adapter: Bridged Mode.

Firewall: Allow the app through Windows Defender Firewall if it uses networking features

Optional: Configure a static IP if needed for communication with host or external apps

🔥 Firewall & Ports
If your app communicates via network:

Go to Control Panel → Windows Defender Firewall → Allow an app through the firewall

Click Change settings, then Allow another app, and select your .exe or bin\Debug\net6.0-windows\YourApp.exe

Ensure both Private and Public checkboxes are ticked

If you're using specific ports (e.g. for a local server or client-server communication):

Open Windows Defender Firewall with Advanced Security

Go to Inbound Rules → New Rule

Choose Port, select TCP or UDP, enter your port (e.g. 8080), then allow connection

🚀 How to Run
Clone the project:

bash
Copier
Modifier
git clone https://github.com/yourusername/yourproject.git
Open the solution in Visual Studio

Restore NuGet packages:

Right-click the solution → "Restore NuGet Packages"

Build the project (Ctrl + Shift + B)

Run (F5)

🧪 Notes
MetroFramework only supports WinForms, not WPF

If you're deploying on another machine, make sure .NET 6.0 Desktop Runtime is installed

Tested on Windows 10 Home and Windows 11 Pro

📜 License & Credits
This project uses:

MetroFramework (Modern UI)
Licensed under the MIT License

MetroFramework by Dennis Magno, GitHub Repository

📞 Contact
For questions or bugs, please open an issue or contact me directly via GitHub.

vbnet
Copier
Modifier

Let me know if you want this customized further (like project name, specific ports, or screenshots)!
