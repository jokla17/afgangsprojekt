import { Icon } from "../icon/icon";
import { CompumatType } from "./compumatType.enum";

export interface Compumat {
  id: string,
  name: string,
  latitude: number,
  longitude: number,
  type: CompumatType,
  icon: Icon,
  status: string,
}
