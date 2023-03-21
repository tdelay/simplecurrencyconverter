import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Rate } from './models/rates.model';

@Injectable({
  providedIn: 'root'
})
export class EcbService {

  constructor(private http: HttpClient) { }
  url = environment.apiUrl;

  GetEcbRates(){

      return this.http.get<Rate[]>(this.url + "api/ecb")

  }

}
