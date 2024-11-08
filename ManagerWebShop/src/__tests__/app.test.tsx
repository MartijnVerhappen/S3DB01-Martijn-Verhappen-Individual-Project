import { render, screen, waitFor } from "@testing-library/react";
import App from "../App";
import axios from 'axios';
import '@testing-library/jest-dom';

// Mock Axios
jest.mock("axios");
const mockedAxios = axios as jest.Mocked<typeof axios>;

describe("App Component", () => {
  beforeEach(() => {
    // Clear any previous mock calls before each test
    mockedAxios.get.mockClear();
  });

  test("displays products after API call", async () => {
    // Mock the Axios response
    mockedAxios.get.mockResolvedValueOnce({
      data: [
        { id: 1, productType: "Videogame", productNaam: "Monster Hunter World", productPrijs: 60, productKorting: 0 },
        { id: 2, productType: "Videogame", productNaam: "God of War", productPrijs: 50, productKorting: 60 }
      ],
    });

    render(<App />);
    // Check if products are displayed
    await waitFor(() => {
      expect(screen.getByText(/Monster Hunter World/i)).toBeInTheDocument();
      expect(screen.getByText(/God of War/i)).toBeInTheDocument();
    });
  });
});