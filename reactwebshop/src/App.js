import './App.css';
import React, { useState } from 'react';
import { Route, Routes, Navigate, useNavigate } from 'react-router-dom';
import ProductList from './ProductList';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const navigate = useNavigate();

  const handleLogin = () => {
    setIsLoggedIn(true);
    navigate('/products');
  };

  return (
    <div className="app">
      <Routes>
        <Route path="/products" element={isLoggedIn ? <ProductList /> : <Navigate to="/" />} />
        <Route path="/" element={
          <div className="login-page">
            <h1>Welcome to the Webshop</h1>
            <button onClick={handleLogin}>Login</button>
          </div>
        } />
      </Routes>
    </div>
  );
}

export default App;
