import React from 'react';
import { Box, Drawer, List, ListItem, ListItemIcon, ListItemText, Typography, TextField, Button, Card, CardContent, CardMedia, Divider, ListItemButton } from '@mui/material';
import { styled } from '@mui/material/styles';
import { useNavigate, useLocation } from 'react-router-dom';
import DashboardIcon from '@mui/icons-material/Dashboard';
import EditIcon from '@mui/icons-material/Edit';
import AnalyticsIcon from '@mui/icons-material/Analytics';
import CommentIcon from '@mui/icons-material/Comment';
import SettingsIcon from '@mui/icons-material/Settings';
import SearchIcon from '@mui/icons-material/Search';
import FavoriteBorderIcon from '@mui/icons-material/FavoriteBorder';
import ChatBubbleOutlineIcon from '@mui/icons-material/ChatBubbleOutline';
import MoreHorizIcon from '@mui/icons-material/MoreHoriz';
import CategoryIcon from '@mui/icons-material/Category';
import LocalOfferIcon from '@mui/icons-material/LocalOffer';
import ImageIcon from '@mui/icons-material/Image';

// Стилі для основного контейнера
const DashboardContainer = styled(Box)(({ theme }) => ({
    display: 'flex',
    minHeight: '100vh',
    backgroundColor: '#F5F7FA',
}));

// Стилі для бічної панелі
const Sidebar = styled(Drawer)(({ theme }) => ({
    width: 240,
    flexShrink: 0,
    '& .MuiDrawer-paper': {
        width: 240,
        boxSizing: 'border-box',
        backgroundColor: '#fff',
        borderRight: '1px solid #E0E0E0',
    },
}));

// Стилі для основного контенту
const Content = styled(Box)(({ theme }) => ({
    flexGrow: 1,
    padding: theme.spacing(3),
}));

// Стилі для кнопки "New Post"
const NewPostButton = styled(Button)(({ theme }) => ({
    backgroundColor: '#1A3C34',
    color: '#fff',
    '&:hover': {
        backgroundColor: '#2A5C54',
    },
}));

// Стилі для картки посту
const PostCard = styled(Card)(({ theme }) => ({
    marginBottom: theme.spacing(2),
    boxShadow: theme.shadows[1],
}));

const DashboardPage: React.FC = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const userName = 'JOHN';

    const menuItems = [
        { text: 'Dashboard', icon: <DashboardIcon />, path: '/dashboard' },
        { text: 'Content Editor', icon: <EditIcon />, path: '/editor' },
        { text: 'Categories', icon: <CategoryIcon />, path: '/categories' },
        { text: 'Tags', icon: <LocalOfferIcon />, path: '/tags' },
        { text: 'Media Library', icon: <ImageIcon />, path: '/media' },
        { text: 'Analytics', icon: <AnalyticsIcon />, path: '/analytics' },
        { text: 'Comments', icon: <CommentIcon />, path: '/comments' },
        { text: 'Settings', icon: <SettingsIcon />, path: '/settings' },
    ];

    return (
        <DashboardContainer>
            {/* Бічна панель навігації */}
            <Sidebar variant="permanent" anchor="left">
                <Box p={2}>
                    <Typography variant="h6">
                        ContentFlow CMS
                    </Typography>
                </Box>
                <List>
                    {menuItems.map((item) => (
                        <ListItemButton 
                            key={item.text} 
                            selected={location.pathname === item.path}
                            onClick={() => navigate(item.path)}
                        >
                            <ListItemIcon>{item.icon}</ListItemIcon>
                            <ListItemText primary={item.text} />
                        </ListItemButton>
                    ))}
                </List>
            </Sidebar>

            {/* Основний контент */}
            <Content>
                {/* Привітання та пошук */}
                <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
                    <Typography variant="h5">
                        Welcome back, {userName}!
                    </Typography>
                    <Box display="flex" alignItems="center">
                        <TextField
                            variant="outlined"
                            placeholder="Search content..."
                            InputProps={{
                                startAdornment: <SearchIcon color="action" />,
                            }}
                            sx={{ mr: 2 }}
                        />
                        <NewPostButton variant="contained">
                            + New Post
                        </NewPostButton>
                    </Box>
                </Box>

                {/* Прев'ю посту */}
                <PostCard>
                    <CardMedia
                        component="div"
                        sx={{ height: 200, backgroundColor: '#E0E0E0', display: 'flex', alignItems: 'center', justifyContent: 'center' }}
                    >
                        <Typography color="textSecondary">Featured Image</Typography>
                    </CardMedia>
                </PostCard>

                {/* Чернетка */}
                <PostCard>
                    <CardContent>
                        <Typography variant="h6">
                            Getting Started with Content Management
                        </Typography>
                        <Typography variant="body2" color="textSecondary">
                            Draft • Updated 2h ago
                        </Typography>
                        <Box display="flex" justifyContent="space-between" mt={1}>
                            <Box display="flex" alignItems="center">
                                <FavoriteBorderIcon fontSize="small" sx={{ mr: 1 }} />
                                <ChatBubbleOutlineIcon fontSize="small" />
                            </Box>
                            <MoreHorizIcon />
                        </Box>
                    </CardContent>
                </PostCard>
            </Content>
        </DashboardContainer>
    );
};

export default DashboardPage;