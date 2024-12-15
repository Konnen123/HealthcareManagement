import {UserDto} from './user.dto';

export interface StaffDto extends UserDto
{
  medicalRank: string,
}
