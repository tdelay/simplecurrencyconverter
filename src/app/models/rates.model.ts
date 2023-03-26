export class Rate{
  constructor( _id:number, _title:string,  _rate: number){
    this.id = _id,
    this.rate = _rate,
    this.currency = _title
  }

  id: number;
  currency: string;
  rate: number;
 // selected!: boolean;
}
