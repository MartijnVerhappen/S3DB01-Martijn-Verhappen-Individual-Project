import './ProductCard.css';

function ProductCard({ product }) {
    console.log(product);
    return (
        <div className="product-card" key={product.id}>
        <img src={product.imageUrl} alt={product.productNaam} />
        <h3>{product.productNaam}</h3>
        <p>{product.productType}</p>
        <p>Prijs: ${product.productPrijs}</p>
        <p>Korting: {product.productKorting}%</p>
        <button>Add to Cart</button>
      </div>
    );
  }

export default ProductCard;