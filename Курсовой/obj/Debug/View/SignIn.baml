   M S B A M L   `   `   `  ����K  GКурсовой, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:Курсовой.ClassesКурсовой.Classes  5   	   G(clr-namespace:Курсовой.ViewModelКурсовой.ViewModel  5   	   =#clr-namespace:Курсовой.ViewКурсовой.View  5   	   "    Курсовой.View.SignIn   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5ff&clr-namespace:Курсовой.Classes     5   	   7vm(clr-namespace:Курсовой.ViewModel     5   	   5local#clr-namespace:Курсовой.View     5   	   -   5        $��
SignInPage��5
   (   $	��400��6	   $	��300��6     � DataContext  5      0  *Курсовой.ViewModel.SignInViewModel  5   
   .��5      �    BackgroundImage#���   6    � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      $	��100 �6   �� 5      $��50 �6   �� 5      $��50 �6   �� 5      $��50 �6   �� 5      $��50 �6   �� 5      $��50 �6   �� 5      $��30 �6   �� 5      $��30 �6   .+�5   
   �� $��1��6     _SignIn#���  6!   $��Center=�6B   $��Center=�6]   $��32(�6z   $
��Bold"�6�    
 Header#���  6�   �� 5   
   -   5        $��Login��5      $��3��6C   $	��300��6P   $��35��6_   ��6   �� *Login+ �� Mode$ TwoWay=�6_   �� 5   
    J� IsHitTestVisible .  $��3��6-   $��16(�6:    
 _Login#���  6H   $	��300��6h    �� TextAlignment$
 Left=�6u   $��35��6�    �� Padding$ 7q�6�   $��Calibri)�6�     ForegroundTextBox#���  6�   ��5      �� 5       �� 
TargetType" ��6   .�5       ��  �� Property � y� �� Value$ 	Collapsed=�63   	 �� Triggers	 5!      �� 5"      
 �� Binding
 6&   �� *Text+ �� ElementName$ Login�� �� Value  6R   .a�5#      ��  � y�$ Visible=�6;   5$      5&      5(      G� 5*   
   -   5        $��Password��5*      $��4��6&   $	��300��63   $��35��6B     	_Password
 � Tag# �  6�   %   Курсовой.Classes.Secure   BoundPassword 6N   �� *
Password+$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5,   
   $��6��6     
MainButton#��[ 6   #���  6B   $	��150��6f   ��6r   �� *SignInClick+��6�   �� $ 
SignInPage��� 5.   
   $��7��6   $��Center=�6!    � Orientation$ Vertical=�6>   $��
10 10 10 0q�6U   .+�5/      ��   	_RegClick#���  6   $
��Wrap=�6;   $��14(�6O   #���  6]   �� 50      $��Center=�6   �� 51      $�� ��6    �� 52      $��Calibri)�6   $��14(�62    �� Command 6@   �� *RegistrationClick+ �� CommandParameter 6f   �� $ 
SignInPage��.�6�   ��   _ClickMe#���  6�   6�   53      54      � 55   
   $��8��6   $��Center=�6!   $ Vertical=�6>   $��	10 5 10 0q�6U   .+�56      ��  	 
_ForgotPas#��� 	 6   $
��Wrap=�6<   $��14(�6P   #���  6^   �� 57      $��Center=�6   �� 58      $�� ��6    �� 59      $��Calibri)�6   $��14(�62    6@   �� *NewPassword+ 6`   �� $ 
SignInPage��.�6�   �� #���  6�   6�   5:      5;      5<      5=      