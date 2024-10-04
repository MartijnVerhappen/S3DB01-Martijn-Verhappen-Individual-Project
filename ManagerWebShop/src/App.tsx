import { useState } from "react";
import "./App.css";



function App() {
  const [responseData, setResponseData] = useState<Product | null>(null);
  const [error, setError] = useState<string | null>(null);

  // function to transform called data into something more usable
  const transformProductData = (data: Product) => {
    return {
      Id: data.Id,

      ProductType: data.ProductType,

      ProductNaam: data.ProductNaam,

      ProductPrijs: data.ProductPrijs,

      ProductKorting: data.ProductKorting
    };
};

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
  
      const { Success, Data } = await getAllResponse.json();
      if (Success) {
          const formattedData = transformProductData(Data); // Transform the data
          setResponseData(formattedData);
      }
      
    } catch (error) {
      // Check if error is apart of ErrorConstructor
      if (error instanceof Error) {
        setError(error.message); // show error message
      } else {
        setError("An unknown error occurred"); // catchall
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

      {/* Display API response or error */}
      <p>{responseData ? JSON.stringify(responseData) : "No data yet"}</p>
      {error && <p>Error: {error}</p>}
    </div>
  );
}

export default App;

