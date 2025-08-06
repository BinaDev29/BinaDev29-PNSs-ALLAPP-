// PNS-FrontEnd/src/pages/AboutPage.tsx
import React from 'react';
import { Typography, Container } from '@mui/material'; // 'Box' removed from here

const AboutPage: React.FC = () => {
  return (
    <Container maxWidth="md" sx={{ mt: 8 }}>
      <Typography variant="h4" component="h1" gutterBottom>
        About Our Push Notification System
      </Typography>
      <Typography variant="body1" paragraph>
        Our Push Notification System (PNS) is a robust and scalable solution
        designed for developers and businesses to connect with their users
        through timely and effective notifications. Built with modern technologies
        like .NET Core, SQL Server, and React with TypeScript, PNS
        offers a seamless experience for managing client applications and delivering
        messages across various platforms.
      </Typography>
      <Typography variant="body1" paragraph>
        Focusing on performance, security, and ease of use, PNS
        aims to streamline your communication efforts, enabling you to deliver
        essential updates, promotions, and personalized content to your audience.
      </Typography>
      <Typography variant="h6" component="h2" sx={{ mt: 4, mb: 2 }}>
        Key Features:
      </Typography>
      <ul>
        <li><Typography variant="body1">Secure API Key-based Authentication</Typography></li>
        <li><Typography variant="body1">Client Application Management (CRUD operations)</Typography></li>
        <li><Typography variant="body1">Targeted Notification Delivery to specific users or applications</Typography></li>
        <li><Typography variant="body1">Scalable and Reliable Backend Infrastructure</Typography></li>
        <li><Typography variant="body1">Intuitive Admin Dashboard for easy management</Typography></li>
      </ul>
    </Container>
  );
};

export default AboutPage;