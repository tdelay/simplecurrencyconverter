import { Component, OnInit } from '@angular/core';
import { EcbService } from '../ecb.service';
import { Rate } from '../models/rates.model';

@Component({
  selector: 'app-currencyconverter',
  templateUrl: './currencyconverter.component.html',
  styleUrls: ['./currencyconverter.component.scss']
})
export class CurrencyconverterComponent implements OnInit {
  baseRate: number | undefined = 1; //initial value is EUR rate = 1
  foreignRate: number | undefined = 0; // this value is unknown so default value is zero
  isLoading = true;
  exchangedValue: number | undefined= 0;
  fromCurrency = "EUR";
  toCurrency= "USD"
  showError = false;
  errorMessage = "Unable to access API check your network connection and ensure that API is up and running"
  ratesModel: Rate[] = [];
  constructor(private ecbService: EcbService) {

  }


  clearBaseRate(){

    this.baseRate = 1;
    this.calculateRate(this.baseRate as number, this.foreignRate as number);
  }


  ngOnInit(){

     this.ecbService.GetEcbRates().subscribe(response=>{
      console.log(response);
      this.ratesModel = response;
      console.log(this.ratesModel)
      const initalValue = this.ratesModel.find(c=> c.currency == 'USD')?.rate;

      this.foreignRate = initalValue;
      this.exchangedValue = initalValue;
      this.isLoading = false;
  //     let parser = new DOMParser();
  //     let rateXml = parser.parseFromString(response, "text/xml");

  //     let rates = rateXml.getElementsByTagName("Cube");

  //     //add initial EUR rate cause it doesnt exist in the XML
  //     this.ratesModel.push(new Rate(1,'EUR',1))
  //     for(let i = 0; i < rates.length; i++){

  //         var rate = rates[i];
  //         var currency = rate.getAttribute("currency")
  //         var currencyRate = rate.getAttribute("rate");
  //         if(currency != null && currencyRate != null){

  //            let currencyItem = new Rate(i,currency,Number(currencyRate));

  //            if(currency == "USD"){
  //             this.foreignRate = Number(currencyRate);
  //             this.exchangedValue = Number(currencyRate)
  //            }



  //           this.ratesModel.push(currencyItem);

  //       }
  //     }

   },error =>{
      this.showError = true;
   });

  }


  convertBaseCurrency(event: any){
    console.log(this.baseRate);
    console.log(this.foreignRate);
    console.log(this.fromCurrency);
    console.log(this.toCurrency);
    const foreignRateImmutable = this.ratesModel.find(c=> c.currency == this.toCurrency)?.rate;
    // if(this.foreignRate == 0 || this.foreignRate == null)
    //   this.foreignRate = foreignRateImmutable;



    this.calculateRate(this.baseRate as number, foreignRateImmutable as number);

  }

  convertForeignCurrency(event: any){
    console.log(this.baseRate);
    console.log(this.foreignRate);
    console.log(this.fromCurrency);
    console.log(this.toCurrency);
    const foreignRateImmutable = this.ratesModel.find(c=> c.currency == this.toCurrency)?.rate;
    // if(this.foreignRate == 0 || this.foreignRate == null)
    //   this.foreignRate = foreignRateImmutable;



    this.calculateRate(this.baseRate as number, foreignRateImmutable as number);

  }

  // initialCurrencySelect(event: any){
  //   if(event.value != null)
  //       this.baseRate = this.ratesModel.find(c=> c.currency == event.value)?.rate;
  // }


  exchangedCurrencySelect(event: any){
    if(event.value != null)
        this.foreignRate = this.ratesModel.find(c=> c.currency == event.value)?.rate;

    this.calculateRate(this.baseRate as number, this.foreignRate as number);
  }


  calculateRate(baseCurrency: number , foreignCurrency: number, ){

    console.log(foreignCurrency);

    let exchangeRate = baseCurrency * foreignCurrency;

    this.exchangedValue = exchangeRate;


  }

}
