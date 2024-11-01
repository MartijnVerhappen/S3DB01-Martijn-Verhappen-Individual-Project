import { useEffect, useState } from "react";
import axios from "axios";
import "./App.css";
import { Product } from "./Product";

function App() {
  const [responseData, setResponseData] = useState<Product[]>([]);
  const [selectedProduct, setSelectedProduct] = useState<Product | null>(null);
  const [error, setError] = useState<string | null>(null);

  const callApi = async () => {
    try {
      const apiUrl = "http://localhost:5000/Product";
      const getAllResponse = await axios.get(apiUrl);
      setResponseData(getAllResponse.data);
    } catch (error) {
      if (axios.isAxiosError(error)) {
        setError(error.message);
      } else {
        setError("An unknown error occurred");
      }
    }
  };

  const handleUpdateProduct = async () => {
    if (selectedProduct) {
      try {
        const apiUrl = `http://localhost:5000/Product/Update?id=${selectedProduct.id}`;
        const response = await axios.put(apiUrl, selectedProduct);
        console.log(response.data);
        callApi();
        setSelectedProduct(null);
      } catch (error) {
        if (axios.isAxiosError(error)) {
          setError(error.message);
        } else {
          setError("An unknown error occurred");
        }
      }
    }
  };

  useEffect(() => {
    callApi();
  }, []);

  return (
    <div className="container">
      <h1>Welcome to the online web shop!</h1>
  
      {responseData && responseData.length > 0 ? (
        <div className="product-list">
          <div className="product-header">
            <div>Product Name</div>
            <div>Product Type</div>
            <div>Price</div>
            <div>Discount</div>
            <div>Actions</div>
          </div>
  
          {responseData.map((product: Product) => (
            <div className="product-item" key={product.id}>
              <div>{product.productNaam}</div>
              <div>{product.productType}</div>
              <div>${product.productPrijs}</div>
              <div>%{product.productKorting}</div>
              <div>
                <button onClick={() => setSelectedProduct(product)}>Edit</button>
              </div>
            </div>
          ))}
        </div>
      ) : (
        <p>No data yet</p>
      )}
  
      {selectedProduct && (
        <div>
          <h2>Edit Product</h2>
          <div style={{ display: 'flex', gap: '10px' }}>
            <input
              type="text"
              value={selectedProduct.productNaam}
              onChange={(e) => setSelectedProduct({ ...selectedProduct, productNaam: e.target.value })}
              placeholder="Product Name"
            />
            <input
              type="text"
              value={selectedProduct.productType}
              onChange={(e) => setSelectedProduct({ ...selectedProduct, productType: e.target.value })}
              placeholder="Product Type"
            />
            <input
              type="number"
              value={selectedProduct.productPrijs}
              onChange={(e) => setSelectedProduct({ ...selectedProduct, productPrijs: Number(e.target.value) })}
              placeholder="Price"
            />
            <input
              type="number"
              value={selectedProduct.productKorting}
              onChange={(e) => setSelectedProduct({ ...selectedProduct, productKorting: Number(e.target.value) })}
              placeholder="Discount"
            />
            <button onClick={handleUpdateProduct}>Save Changes</button>
          </div>
        </div>
      )}
      
      {error && <p>Error: {error}</p>}
    </div>
  );
  
}

export default App;
