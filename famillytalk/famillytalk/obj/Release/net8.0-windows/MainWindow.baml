   M S B A M L   `   `   `  ����   famillytalk    famillytalk.MainWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     T PMahApps.Metro, Version=2.0.0.0, Culture=neutral, PublicKeyToken=51482d6f650b2b3f;controls,http://metro.mahapps.com/winfx/xaml/controls    � Title$  Facepasbook��$	��450��$	��800�� � WindowStartupLocation$ CenterScreen=�   AccentColorBrush(  "MahApps.Metro.Controls.MetroWindow   	GlowBrush# �    � WindowStyle$
 None=� � 
ResizeMode$ NoResize=�.��� $��10q� � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+���  �� Orientation$ 
Horizontal=�$��0��$��0,0,0,10q�$��Center=�.+��� $��Votre pseudo:��$��Center=��� -   $��	PseudoBox��$	��200��$	��5,0q��� $��	IP cible:��$��Center=�$��10,0,0,0q��� -   $��TargetIPBox��$	��120��$	��5,0q��� -   $��24��$��24��$	��5,0q� � ToolTip$ Ajouter IP en cible�� � Cursor$
 Hand��
�������
�������$��1q�$��0q����� 	 �� 
TargetType"	 ��.R�� .+�A� #��z��#��z��#��z���� $��+��$
��Bold"�
���   �$��16(�$��Center=�$��Center=��� -   $��ConnectButton��$��Se connecter��$	��143��$
��10,0q��� -   $��ChatHistory��$��1��
 �� 
IsReadOnly$

 True��" �� VerticalScrollBarVisibility$
 Auto=� �� TextWrapping$
 Wrap=� �� AcceptsReturn$
 True��$��14(��� $ 
Horizontal=�$��2��$��0,10,0,0q�.+��� -   $��MessageInput��$	��625��$��30��$��0,0,10,0q�$ False���� -    ��Name$ 
SendButton��$��Envoyer��$	��145�� �� 	IsDefault$
 True��