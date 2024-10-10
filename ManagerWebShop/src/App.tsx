import { useState } from "react";
import "./App.css";

function App() {
  const [responseData, setResponseData] = useState<Product[]>([]);
  const [error, setError] = useState<string | null>(null);

  // Function to call the API
  const callApi = async () => {
    try {
      const apiUrl = "http://localhost:5000/Product";
      const getAllResponse = await fetch(apiUrl, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      });
  
      if (!getAllResponse.ok) {
        throw new Error(`HTTP error! status: ${getAllResponse.status}`);
      }
  
      const productsResponse = await getAllResponse.json();
      setResponseData(productsResponse); // Directly set the response data
  
    } catch (error) {
      if (error instanceof Error) {
        setError(error.message);
      } else {
        setError("An unknown error occurred");
      }
    }
  };
  
  

  return (
    <div className="container">
      <h1>Welcome to the online web shop!</h1>
  
      <div className="row"></div>
  
      <form
        onSubmit={(e) => {
          e.preventDefault(); // Prevent the form from refreshing the page
          callApi(); // Call the API when form is submitted
        }}
      >
        <button type="submit">Call API</button>
      </form>
  
      {/* Display API response */}
      {responseData && responseData.length > 0 ? (
        <ul>
          {responseData.map((product: Product) => (
            <li key={product.id}>
              <p>Product Name: {product.productNaam}</p>
              <p>Product Type: {product.productType}</p>
              <p>Price: ${product.productPrijs}</p>
              <p>Discount: %{product.productKorting}</p>
            </li>
          ))}
        </ul>
      ) : (
        <p>No data yet</p>
      )}
  
      {/* Display error message */}
      {error && <p>Error: {error}</p>}
    </div>
  );
  
}

export default App;