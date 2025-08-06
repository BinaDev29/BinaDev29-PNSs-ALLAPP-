// PNS-FrontEnd/src/app/components/PublicLayout.tsx
import React from 'react';
import { Outlet, Link as RouterLink } from 'react-router-dom';
import { AppBar, Toolbar, Typography, Button, Box, Container } from '@mui/material';

const PublicLayout: React.FC = () => {
  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
      <AppBar position="static"> {/* AppBar - Top Navigation Bar */}
        <Toolbar> {/* Toolbar */}
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            PNS - Push Notification System
          </Typography>
          {/* Navigation Links */}
          <Button color="inherit" component={RouterLink} to="/">Home</Button>
          <Button color="inherit" component={RouterLink} to="/about">About Us</Button>
          <Button color="inherit" component={RouterLink} to="/contact">Contact</Button>
          <Button color="inherit" component={RouterLink} to="/login">Login</Button>
        </Toolbar>
      </AppBar>
      <Container component="main" sx={{ flexGrow: 1, py: 4 }}>
        <Outlet /> {/* Displays child route components (HomePage, AboutPage, etc.) */}
      </Container>
      <Box component="footer" sx={{ p: 3, bgcolor: 'primary.main', color: 'white', textAlign: 'center' }}>
        <Typography variant="body2">
          &copy; {new Date().getFullYear()} PNS. All rights reserved.
        </Typography>
      </Box>
    </Box>
  );
};

export default PublicLayout;