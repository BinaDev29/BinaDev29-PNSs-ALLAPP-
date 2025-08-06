// PNS-FrontEnd/src/features/auth/pages/LoginPage.tsx
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { TextField, Button, Box, Typography, Container } from '@mui/material';
import { useAuth } from '../../../app/providers/useAuth'; // Import useAuth from useAuth.ts
import axios from 'axios'; // Import Axios

const API_BASE_URL = 'http://localhost:5000/api'; // Backend API URL

const LoginPage: React.FC = () => {
  const [apiKey, setApiKey] = useState('');
  const [error, setError] = useState<string | null>(null);
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    setError(null); // Clear previous error

    if (!apiKey) {
      setError('You must enter an API Key.');
      return;
    }

    try {
      // Make a call to validate the API Key
      const response = await axios.post(`${API_BASE_URL}/auth/validate-api-key`, { apiKey });

      if (response.status === 200 && response.data.isValid) {
        login(apiKey); // Store API Key if valid
        navigate('/admin/client-applications', { replace: true }); // Navigate to dashboard
      } else {
        setError('Invalid API Key. Please try again.');
      }
    } catch (err) {
      setError('Failed to validate API Key.');
      console.error('Login error:', err);
    }
  };

  return (
    <Container component="main" maxWidth="xs">
      <Box
        sx={{
          marginTop: 8,
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          p: 4,
          boxShadow: 3,
          borderRadius: 2,
          bgcolor: 'background.paper',
        }}
      >
        <Typography component="h1" variant="h5">
          Login
        </Typography>
        <Box component="form" onSubmit={handleSubmit} noValidate sx={{ mt: 1 }}>
          <TextField
            margin="normal"
            required
            fullWidth
            id="apiKey"
            label="API Key"
            name="apiKey"
            autoComplete="current-password"
            autoFocus
            value={apiKey}
            onChange={(e) => setApiKey(e.target.value)}
          />
          {error && (
            <Typography color="error" variant="body2" sx={{ mt: 1 }}>
              {error}
            </Typography>
          )}
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            Login
          </Button>
        </Box>
      </Box>
    </Container>
  );
};

export default LoginPage;