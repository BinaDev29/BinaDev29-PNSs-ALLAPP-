// PNS-FrontEnd/src/app/App.tsx
import React from 'react';
import AppRoutes from './router'; // Our routing configuration

const App: React.FC = () => {
  return (
    <>
      <AppRoutes /> {/* All routes are defined here */}
    </>
  );
};

export default App;