// PNS-FrontEnd/src/features/notifications/pages/NotificationDashboard.tsx
import React from 'react';
import { Box, Typography, Container } from '@mui/material';

const NotificationDashboard: React.FC = () => {
  return (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Typography variant="h4" component="h1" gutterBottom>
        ማሳወቂያዎችን ይላኩ
      </Typography>
      <Typography variant="body1" paragraph>
        ይህን ገጽ በመጠቀም ለተመዘገቡ የደንበኛ አፕሊኬሽኖችዎ እና
        ከነሱ ጋር ለተያያዙ የኢሜል ተቀባዮች ፑሽ ማሳወቂያዎችን ይላኩ።
      </Typography>
      {/* TODO: የማሳወቂያ መላኪያ ፎርም እዚህ ያክሉ */}
      <Box sx={{ p: 3, border: '1px dashed grey', mt: 3 }}>
        <Typography variant="h6">የማሳወቂያ መላኪያ ፎርም በቅርቡ ይመጣል...</Typography>
      </Box>
    </Container>
  );
};

export default NotificationDashboard;