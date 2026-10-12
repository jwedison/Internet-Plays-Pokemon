using Avalonia;
using Avalonia.Controls;

namespace internetplays.Views;

public partial class MainWindow : Window
{
    private readonly ScrollViewer _chatScroller;

    public MainWindow()
    {
        InitializeComponent();

        this.FindControl<ItemsControl>("ChatMessages").PropertyChanged += ChatMessagesPropertyChanged;
        _chatScroller = ChatScroller;
    }

    private void ChatMessagesPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        _chatScroller.ScrollToEnd();
    }
}