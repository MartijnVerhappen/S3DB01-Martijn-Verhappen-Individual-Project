import './ProductList.css';
import React, { useState, useEffect } from 'react';
import ProductCard from './ProductCard';

function ProductList() {
    const [products, setProducts] = useState(new Set());
  
    useEffect(() => {
      const fetchProducts = async () => {
        try {
          const response = await fetch('http://localhost:5000/product');
          const data = await response.json();
          // Create a new Set to remove duplicates
          const uniqueProducts = new Set(data);
          setProducts(uniqueProducts);
        } catch (error) {
          console.error('Error fetching products:', error);
        }
      };
      fetchProducts();
    }, []);
  
    return (
      <div className="product-list">
        {Array.from(products).map((product) => (
          <ProductCard key={product.id} product={product} />
        ))}
      </div>
    );
  }  

export default ProductList;