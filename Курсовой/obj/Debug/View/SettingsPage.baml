   M S B A M L   `   `   `  ����K  GКурсовой, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:Курсовой.ViewКурсовой.View  5      G(clr-namespace:Курсовой.ViewModelКурсовой.ViewModel  5      C&clr-namespace:Курсовой.ClassesКурсовой.Classes  5      (    "Курсовой.View.SettingsPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:Курсовой.View     5      7vm(clr-namespace:Курсовой.ViewModel     5      5ff&clr-namespace:Курсовой.Classes     5        N� Title$  SettingsPage��5
       � DataContext 5      6  0Курсовой.ViewModel.SettingsPageViewModel  5   
   .��5      �    BackgroundImage#���   6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��2* �6    � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      $��50 �6   �� 5      $��50 �6   �� 5      $��50 �6   �� 5      $��50 �6   �� 5      $	��200 �6   �� 5      $��* �6   .+�5    
   ��  
 TextBl#���  6   $��1��65     	_Language#���  6B   $	��Top=�6e   $��20(�6}   $��Right=�6�   $
��10,0q�6�   �� 5!   
   $��1��6-   $��1��6:   $��18(�6J   $��30��6X   $	��Top=�6d   $��0 0 20 0q�6|   $
��Left=�6�   $	��200��6�    �� Text 6   �� *
Language+.@�5"      �� $��English��6   �� 5#      $��Русский��6   5$      �� 5&   
   #���  6   $��2��65    	 _Them#���  6B   $	��Top=�6a   $��20(�6y   $��Right=�6�   $
��10,0q�6�   �� 5'   
   $��2��6)   $��1��66   $��18(�6F   $��30��6T   $	��Top=�6`   $
��Left=�6x   $	��200��6�   $��0 0 20 0q�6�    6   �� *Them+.@�5(      ��   _DefaultThem#���  6   �� 5)        	_DarkThem#���  6   5*      �� 5,   
   #���  6   $
��Wrap=�65   $��3��6I     
_ChangePas#���  6V   $��20(�6z   $	��Top=�6�   $��Right=�6�   $
��10,0q�6�   G� 5-   
   -   5        $��OldPassword��5-   3   $��3��6   $��1��6#   $	��200��6F   $��35��6U   $
��Left=�6a   $	��Top=�6|     	_Password
 � Tag# �  5.   s   %   Курсовой.Classes.Secure   BoundPassword 6   �� *
Password+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6s   G� 50   
   -   5        $��NewPassword��50   3   $��4��6   $��1��6#   $	��200��6F   $��35��6U   $
��Left=�6a   $	��Top=�6|     _NewPas# �  51   v    6   �� *NewPassword+$ TwoWay=�$ PropertyChanged=�6v   �� 53   
    	 _Choose#��� 	 6    
 
MainButton#��[
 65   $��Right=�6Y   $
��10 0q�6u   $��5��6�   $��0��6�   $��40��6�   $	��Top=�6�   $	��100��6�   ��6�   �� *ChooseAvatar+�� 54   
   	 �� CornerRadius$	 10��6   ���	#33828080$��5��6:   $��1��6G   $
��Left=�6W   ._�55      �� $	��Top=�6   $	��200��6F   $	��200��6R   
 J� 	AllowDrop
 . ��6,   �� *Avatar+6_   56      �� 58   
   $��6��6   $��1��6   $
��Left=�6.   $��Bottom=�6I   $��	0 20 0 50q�6d   $��50��6w   #��[
 6�    	 _Save#���  6�   $	��150��6�   ��6�   �� *	SaveClick+5:      5;      