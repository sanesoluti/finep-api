using Microsoft.EntityFrameworkCore;
using apifinep.Data;
using System.Data;
using Npgsql;

namespace apifinep
{
    public class DatabaseAnalyzer
    {
        private readonly ApplicationDbContext _context;
        
        public DatabaseAnalyzer(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task AnalyzePeaksTablesAsync()
        {
            try
            {
                Console.WriteLine("=== ANÁLISE DA TABELA peaks_devices ===");
                var peaksDevicesQuery = @"
                    SELECT 
                        column_name, 
                        data_type, 
                        is_nullable, 
                        column_default,
                        character_maximum_length
                    FROM information_schema.columns 
                    WHERE table_name = 'peaks_devices' 
                    ORDER BY ordinal_position;";
                
                var peaksDevicesResult = await _context.Database.SqlQueryRaw<dynamic>(peaksDevicesQuery).ToListAsync();
                
                Console.WriteLine("=== ANÁLISE DA TABELA peaks ===");
                var peaksQuery = @"
                    SELECT 
                        column_name, 
                        data_type, 
                        is_nullable, 
                        column_default,
                        character_maximum_length
                    FROM information_schema.columns 
                    WHERE table_name = 'peaks' 
                    ORDER BY ordinal_position;";
                
                var peaksResult = await _context.Database.SqlQueryRaw<dynamic>(peaksQuery).ToListAsync();
                
                Console.WriteLine("=== ANÁLISE DAS FOREIGN KEYS ===");
                var foreignKeysQuery = @"
                    SELECT 
                        tc.constraint_name,
                        tc.table_name,
                        kcu.column_name,
                        ccu.table_name AS foreign_table_name,
                        ccu.column_name AS foreign_column_name
                    FROM information_schema.table_constraints AS tc
                    JOIN information_schema.key_column_usage AS kcu
                        ON tc.constraint_name = kcu.constraint_name
                    JOIN information_schema.constraint_column_usage AS ccu
                        ON ccu.constraint_name = tc.constraint_name
                    WHERE tc.constraint_type = 'FOREIGN KEY'
                        AND (tc.table_name = 'peaks_devices' OR tc.table_name = 'peaks');";
                
                var foreignKeysResult = await _context.Database.SqlQueryRaw<dynamic>(foreignKeysQuery).ToListAsync();
                
                Console.WriteLine("=== DADOS DE EXEMPLO ===");
                var sampleDataQuery = @"
                    SELECT pd.*, p.sequence, p.value, p.created_at as peak_created_at
                    FROM peaks_devices pd
                    LEFT JOIN peaks p ON pd.peaks_devices_id = p.id
                    LIMIT 5;";
                
                var sampleDataResult = await _context.Database.SqlQueryRaw<dynamic>(sampleDataQuery).ToListAsync();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao analisar tabelas: {ex.Message}");
            }
        }
    }
}