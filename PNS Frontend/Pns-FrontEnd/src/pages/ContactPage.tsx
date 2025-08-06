// PNS-FrontEnd/src/pages/ContactPage.tsx
import React from 'react';
import { Box, Typography, Container, Link } from '@mui/material';
import EmailIcon from '@mui/icons-material/Email'; // Email icon
import PhoneIcon from '@mui/icons-material/Phone'; // Phone icon
import LocationOnIcon from '@mui/icons-material/LocationOn'; // Location icon

const ContactPage: React.FC = () => {
  return (
    <Container maxWidth="md" sx={{ mt: 8 }}>
      <Typography variant="h4" component="h1" gutterBottom>
        Contact Us
      </Typography>
      <Typography variant="body1" paragraph>
        We'd love to hear from you! Feel free to reach out for questions, feedback, or support.
      </Typography>
      <Box sx={{ mt: 4 }}>
        <Box display="flex" alignItems="center" mb={2}>
          <EmailIcon sx={{ mr: 1 }} />
          <Typography variant="body1">
            Email: <Link href="mailto:support@pnsapp.com">support@pnsapp.com</Link>
          </Typography>
        </Box>
        <Box display="flex" alignItems="center" mb={2}>
          <PhoneIcon sx={{ mr: 1 }} />
          <Typography variant="body1">
            Phone: <Link href="tel:+251912345678">+251 912 345 678</Link>
          </Typography>
        </Box>
        <Box display="flex" alignItems="center" mb={2}>
          <LocationOnIcon sx={{ mr: 1 }} />
          <Typography variant="body1">
            Address: Addis Ababa, Ethiopia
          </Typography>
        </Box>
      </Box>
      <Typography variant="body2" color="text.secondary" sx={{ mt: 4 }}>
        We strive to respond to all inquiries within 24-48 hours.
      </Typography>
    </Container>
  );
};

export default ContactPage;