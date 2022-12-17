import { Compumat } from "../../compumat/compumat";

export interface LogEvent {
    timestamp: string,
    compumat: Compumat,
    message: string,
    threatLevel: string,
}
