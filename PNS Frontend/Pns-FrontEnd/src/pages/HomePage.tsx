// PNS-FrontEnd/src/pages/HomePage.tsx
import React from 'react';
import { Box, Typography, Container, Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';

const HomePage: React.FC = () => {
  const navigate = useNavigate(); // Hook used to change pages

  return (
    <Container maxWidth="md" sx={{ mt: 8, textAlign: 'center' }}>
      <Typography variant="h3" component="h1" gutterBottom>
        Welcome to PNS - The Push Notification System
      </Typography>
      <Typography variant="h6" color="text.secondary" paragraph>
        Empower your applications with seamless and reliable push notifications.
        Manage your client applications and easily send targeted messages.
      </Typography>
      <Box sx={{ mt: 4 }}>
        <Button
          variant="contained" // Button with a filled background
          size="large" // Large size button
          sx={{ mr: 2 }} // Right margin
          onClick={() => navigate('/login')} // Navigate to login page when button is clicked
        >
          Get Started (Admin Login)
        </Button>
        <Button
          variant="outlined" // Button with only a border
          size="large"
          onClick={() => navigate('/about')} // Navigate to About Us page when button is clicked
        >
          Learn More
        </Button>
      </Box>
    </Container>
  );
};

export default HomePage;