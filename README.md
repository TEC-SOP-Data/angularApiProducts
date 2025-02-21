Når du bruger Angular med en .NET API, sker følgende:

Frontend (Angular) sender en HTTP-forespørgsel til API’et.
Backend (.NET API) modtager forespørgslen, henter data fra databasen og sender svaret tilbage til Angular.
Angular modtager svaret og viser dataene på skærmen.
Hvis brugeren redigerer eller tilføjer data, sender Angular en ny HTTP-forespørgsel til API’et for at gemme ændringerne i databasen.


## HENTE OG VISE DATA

1️⃣. API Controller i .NET
Din ProductController i .NET API’et ser sådan ud:
```csharp
[HttpGet]
public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
{
    return await _context.Product.ToListAsync();
}
```
📌 Hvad sker her?

API’en modtager en GET-request fra Angular.
API’en henter alle produkter fra databasen.


2️. ProductService i Angular
For at hente data i Angular, opretter vi en service (product.service.ts) til at håndtere API-kald:

```angular
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Product {
  id: number;
  name: string;
  price: number;
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = 'https://localhost:7031/api/product'; // .NET API URL

  constructor(private http: HttpClient) {}

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl);
  }
}
```
📌 Hvad sker her?

Angular bruger HttpClient til at sende en HTTP GET-request til API’et.
API’et svarer med en liste af produkter (JSON).
Angular konverterer JSON-dataene til en liste af Product-objekter.

API Model ser sådan ud og passer til interface i Angular
```csharp
namespace angularApiProducts.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
    }
}
```

3️. Hent og vis produkter i en Angular-komponent
I product-list.component.ts, skal vi kalde getProducts() for at hente og vise dataene:

```angular
import { Component, OnInit } from '@angular/core';
import { ProductService, Product } from '../../services/product.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.productService.getProducts().subscribe((data) => {
      this.products = data;
    });
  }
}
```
📌 Hvad sker her?

Når komponenten indlæses (ngOnInit()), henter vi data fra API’et.
Når dataene er hentet, opdateres products[], så de kan vises i HTML’en.


4️⃣. Vis produkterne i HTML
I product-list.component.html viser vi produkterne:

<div *ngFor="let product of products">
  <p>{{ product.name }} - {{ product.price }} DKK</p>
</div>
📌 Hvad sker her?

Angular looper over products og viser navn og pris for hvert produkt.
Når products[] ændres, opdateres visningen automatisk.


SENDE OG GEMME DATA

## 1️⃣ API Controller i .NET (PUT-request til at opdatere produkt)

```csharp
[HttpPut("{id}")]
public async Task<IActionResult> UpdateProduct(int id, Products product)
{
    if (id != product.Id)
    {
        return BadRequest();
    }

    _context.Entry(product).State = EntityState.Modified;
    await _context.SaveChangesAsync();
    
    return NoContent();
}
```
📌 Hvad sker her?

API’et modtager en PUT-request med det opdaterede produkt.
API’et opdaterer produktet i databasen og gemmer ændringerne.


2️⃣ . ProductService i Angular (Metode til at opdatere produkt)
Tilføj en updateProduct() metode i product.service.ts:

updateProduct(product: Product): Observable<void> {
  return this.http.put<void>(`${this.apiUrl}/${product.id}`, product);
}

📌 Hvad sker her?

Angular sender en PUT-request til API’et med det opdaterede produkt.
API’et opdaterer produktet i databasen.


3️⃣. Opdater produktpris i Angular-komponenten
Tilføj en metode i product-list.component.ts:
editPrice(product: Product, newPrice: number): void {
  product.price = newPrice; // Opdater pris lokalt
  this.productService.updateProduct(product).subscribe(() => {
    console.log('Produkt opdateret!');
  });
}
📌 Hvad sker her?


4️⃣. Opdaterer produktets pris i Angular.
Sender PUT-request til API’et for at gemme ændringen.

Tilføj inputfelt og knap i HTML
Brug [(ngModel)] til at få brugerens input:

<div *ngFor="let product of products">
  <p>{{ product.name }} - {{ product.price }} DKK</p>

  <input type="number" [(ngModel)]="product.price" placeholder="Ny pris">
  <button (click)="editPrice(product, product.price)">Opdater pris</button>
</div>

📌 Hvad sker her?

Brugeren skriver en ny pris i inputfeltet.
Når de trykker på "Opdater pris", kaldes editPrice(), som gemmer ændringen i API’et.
