import { Injectable, inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  ResolveFn
} from '@angular/router';
import { Member } from '../_models/member';
import { MembersService } from '../_services/members.service';

export const memberDetailedResolver: ResolveFn<Member> = (route:ActivatedRouteSnapshot) => {
  return inject(MembersService).getMember(route.paramMap.get('username')!)
};
