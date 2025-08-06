// PNS-FrontEnd/src/features/email-recipients/pages/EmailRecipientsDashboard.tsx
import React from 'react';
import { Box, Typography, Container } from '@mui/material';

const EmailRecipientsDashboard: React.FC = () => {
  return (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Typography variant="h4" component="h1" gutterBottom>
        የኢሜል ተቀባዮች አስተዳደር
      </Typography>
      <Typography variant="body1" paragraph>
        ይህ ገጽ ከደንበኛ አፕሊኬሽኖችዎ ጋር የተያያዙ የኢሜል ተቀባዮችን እንዲያስተዳድሩ ያስችሎታል።
        ተቀባዮችን ማከል፣ ማየት፣ ማዘመን እና መሰረዝ ይችላሉ።
      </Typography>
      {/* TODO: የኢሜል ተቀባዮች ሠንጠረዥ እና የCRUD አዝራሮችን እዚህ ያክሉ */}
      <Box sx={{ p: 3, border: '1px dashed grey', mt: 3 }}>
        <Typography variant="h6">የኢሜል ተቀባዮች ዝርዝር በቅርቡ ይመጣል...</Typography>
      </Box>
    </Container>
  );
};

export default EmailRecipientsDashboard;