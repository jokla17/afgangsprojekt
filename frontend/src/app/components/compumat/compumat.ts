import { Icon } from "../icon/icon";
import { CompumatType } from "./compumatType.enum";

export interface Compumat {
  id: string | null;
  stationNo: string | null;
  name: string | null;
  latitude: number | null;
  longitude: number | null;
  type: CompumatType | null;
  icon: Icon | null;
  status: string | null;
}
